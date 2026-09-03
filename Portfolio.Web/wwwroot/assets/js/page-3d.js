import * as THREE from 'https://cdn.jsdelivr.net/npm/three@0.180.0/build/three.module.js';

/* Subtle, route-specific WebGL backgrounds for pages beyond the home hero. */
const host = document.getElementById('page-webgl');
if (host && !window.matchMedia('(prefers-reduced-motion: reduce)').matches) {
    try {
        const route = (document.body.dataset.page || '').toLowerCase();
        const compact = window.matchMedia('(max-width: 700px)').matches;
        const kind = route.includes('contact') ? 'contact'
            : route.includes('about') || route.includes('resume') || route.includes('education') || route.includes('experience') ? 'timeline'
            : route.includes('blog') || route.includes('project') ? 'blocks'
            : route.includes('home') ? 'network' : 'grid';
        const palette = kind === 'contact' ? 0x67e8f9 : kind === 'blocks' ? 0xa3e635 : 0xbef264;
        const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true, powerPreference: 'low-power' });
        renderer.setPixelRatio(Math.min(devicePixelRatio || 1, compact ? 1.15 : 1.5));
        renderer.setClearColor(0x000000, 0);
        renderer.outputColorSpace = THREE.SRGBColorSpace;
        host.appendChild(renderer.domElement);

        const scene = new THREE.Scene();
        const camera = new THREE.PerspectiveCamera(43, 1, 0.1, 100);
        camera.position.z = 13;
        const system = new THREE.Group();
        system.position.set(kind === 'timeline' ? 4.5 : 0, 0, -2);
        scene.add(system);

        const starCount = compact ? 35 : 85;
        const starPositions = new Float32Array(starCount * 3);
        for (let i = 0; i < starCount; i++) {
            starPositions[i * 3] = ((i * 19) % 23 - 11) * 0.62;
            starPositions[i * 3 + 1] = ((i * 13) % 19 - 9) * 0.65;
            starPositions[i * 3 + 2] = -1 - (i % 7) * 0.6;
        }
        const starsGeometry = new THREE.BufferGeometry();
        starsGeometry.setAttribute('position', new THREE.BufferAttribute(starPositions, 3));
        const stars = new THREE.Points(starsGeometry, new THREE.PointsMaterial({ color: palette, size: compact ? 0.035 : 0.05, transparent: true, opacity: 0.6 }));
        system.add(stars);

        const wire = new THREE.LineBasicMaterial({ color: palette, transparent: true, opacity: 0.27 });
        const objects = [];
        if (kind === 'timeline') {
            const segments = [];
            for (let i = 0; i < 7; i++) {
                const x = (i - 3) * 0.9;
                segments.push(x, -4.8, 0, x, 4.8, 0, x - 0.16, i % 2 ? 1.5 : -1.5, 0, x + 0.16, i % 2 ? 1.5 : -1.5, 0);
            }
            const geometry = new THREE.BufferGeometry();
            geometry.setAttribute('position', new THREE.Float32BufferAttribute(segments, 3));
            system.add(new THREE.LineSegments(geometry, wire));
        } else if (kind === 'blocks') {
            for (let i = 0; i < (compact ? 7 : 15); i++) {
                const card = new THREE.Mesh(new THREE.BoxGeometry(1.05, 0.62, 0.08), new THREE.MeshBasicMaterial({ color: palette, wireframe: true, transparent: true, opacity: 0.3 }));
                card.position.set(((i * 17) % 9 - 4) * 1.35, ((i * 11) % 7 - 3) * 1.05, -1 - (i % 4) * 0.5);
                card.rotation.set(i * 0.18, i * 0.31, i * 0.12);
                system.add(card); objects.push(card);
            }
        } else if (kind === 'contact') {
            for (let i = 0; i < 4; i++) {
                const ring = new THREE.Mesh(new THREE.TorusGeometry(1.2 + i * 0.62, 0.012, 4, 80), new THREE.MeshBasicMaterial({ color: palette, transparent: true, opacity: 0.28 }));
                ring.rotation.set(0.7 + i * 0.32, i * 0.45, i * 0.24);
                system.add(ring); objects.push(ring);
            }
        } else if (kind === 'grid') {
            const grid = new THREE.GridHelper(18, 18, palette, palette);
            grid.material.transparent = true;
            grid.material.opacity = 0.18;
            grid.rotation.x = 1.15;
            grid.position.y = -3.8;
            system.add(grid); objects.push(grid);
        } else {
            const orbit = new THREE.Mesh(new THREE.TorusGeometry(3.8, 0.012, 4, 96), new THREE.MeshBasicMaterial({ color: palette, transparent: true, opacity: 0.25 }));
            orbit.rotation.set(1.1, -0.3, 0);
            system.add(orbit); objects.push(orbit);
        }

        scene.add(new THREE.HemisphereLight(0xeaffc8, 0x090c0a, 1.2));
        function resize() {
            const width = host.clientWidth, height = host.clientHeight;
            if (!width || !height) return;
            camera.aspect = width / height;
            camera.updateProjectionMatrix();
            renderer.setSize(width, height, false);
        }
        const resizeObserver = new ResizeObserver(resize);
        resizeObserver.observe(host); resize();
        let visible = true, frame;
        const viewObserver = new IntersectionObserver(([entry]) => { visible = entry.isIntersecting; });
        viewObserver.observe(host);
        function render(time) {
            frame = requestAnimationFrame(render);
            if (!visible) return;
            stars.rotation.z = time * 0.000025;
            objects.forEach((object, index) => { object.rotation.z += 0.00022 * (index % 2 ? -1 : 1); });
            renderer.render(scene, camera);
        }
        render(0);
        window.addEventListener('pagehide', () => {
            cancelAnimationFrame(frame); resizeObserver.disconnect(); viewObserver.disconnect(); renderer.dispose();
        }, { once: true });
    } catch (error) {
        console.warn('The page background could not start.', error);
        host.remove();
    }
}
