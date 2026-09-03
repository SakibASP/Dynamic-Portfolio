import * as THREE from 'https://cdn.jsdelivr.net/npm/three@0.180.0/build/three.module.js';

/* Interactive 3D architecture graph for the portfolio hero. */
const host = document.getElementById('hero-webgl');
const reduceMotion = window.matchMedia('(prefers-reduced-motion: reduce)');

if (host && !reduceMotion.matches) {
    try {
        const compact = window.matchMedia('(max-width: 700px)').matches;
        const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true, powerPreference: 'high-performance' });
        renderer.setPixelRatio(Math.min(window.devicePixelRatio || 1, compact ? 1.25 : 1.75));
        renderer.setClearColor(0x000000, 0);
        renderer.outputColorSpace = THREE.SRGBColorSpace;
        host.appendChild(renderer.domElement);

        const scene = new THREE.Scene();
        const camera = new THREE.PerspectiveCamera(38, 1, 0.1, 100);
        camera.position.set(0, 0, 15);
        const graph = new THREE.Group();
        graph.position.set(compact ? 0.4 : 4.15, 0, 0);
        scene.add(graph);

        const nodeCount = compact ? 30 : 56;
        const nodes = [];
        for (let i = 0; i < nodeCount; i++) {
            const angle = i * 2.39996;
            const radius = 0.75 + Math.sqrt(i / nodeCount) * 3.2;
            nodes.push({
                base: new THREE.Vector3(Math.cos(angle) * radius, Math.sin(angle * 1.7) * 2.5, Math.sin(angle) * radius * 0.55),
                phase: i * 0.73,
                speed: 0.3 + (i % 5) * 0.04
            });
        }

        const nodePositions = new Float32Array(nodeCount * 3);
        const nodeGeometry = new THREE.BufferGeometry();
        nodeGeometry.setAttribute('position', new THREE.BufferAttribute(nodePositions, 3));
        const nodeCloud = new THREE.Points(nodeGeometry, new THREE.PointsMaterial({
            color: 0xd9f99d, size: compact ? 0.075 : 0.095, transparent: true, opacity: 0.96, sizeAttenuation: true
        }));
        graph.add(nodeCloud);

        const links = [];
        for (let i = 1; i < nodeCount; i++) {
            links.push([i, Math.floor((i - 1) / 2)]);
            if (i > 5 && i % 3 === 0) links.push([i, i - 5]);
        }
        const linkPositions = new Float32Array(links.length * 6);
        const linkGeometry = new THREE.BufferGeometry();
        linkGeometry.setAttribute('position', new THREE.BufferAttribute(linkPositions, 3));
        const linkMesh = new THREE.LineSegments(linkGeometry, new THREE.LineBasicMaterial({ color: 0x84cc16, transparent: true, opacity: 0.33 }));
        graph.add(linkMesh);

        // Bright packets travel along connections: a readable visual for request/data flow.
        const packetCount = compact ? 8 : 18;
        const packetPositions = new Float32Array(packetCount * 3);
        const packetGeometry = new THREE.BufferGeometry();
        packetGeometry.setAttribute('position', new THREE.BufferAttribute(packetPositions, 3));
        const packets = new THREE.Points(packetGeometry, new THREE.PointsMaterial({
            color: 0x67e8f9, size: compact ? 0.1 : 0.13, transparent: true, opacity: 0.96, sizeAttenuation: true
        }));
        graph.add(packets);

        // Three dimensional interface planes create a technical, architectural depth.
        const panelMaterial = new THREE.MeshBasicMaterial({ color: 0xbef264, transparent: true, opacity: 0.1, wireframe: true, side: THREE.DoubleSide });
        [[-2.9, 1.9, -1.5, 0.28], [2.85, -1.4, -1.2, -0.36], [0.4, 3.0, -2.2, 0.05]].forEach(([x, y, z, rotation]) => {
            const panel = new THREE.Mesh(new THREE.PlaneGeometry(1.8, 1.06, 7, 4), panelMaterial);
            panel.position.set(x, y, z);
            panel.rotation.y = rotation;
            graph.add(panel);
        });

        const orbit = new THREE.Mesh(new THREE.TorusGeometry(3.85, 0.012, 4, 96), new THREE.MeshBasicMaterial({ color: 0xbef264, transparent: true, opacity: 0.3 }));
        orbit.rotation.x = 1.15;
        orbit.rotation.y = -0.2;
        graph.add(orbit);

        scene.add(new THREE.HemisphereLight(0xe7ffc1, 0x101510, 1.5));
        const lime = new THREE.PointLight(0xbef264, 20, 17, 2);
        lime.position.set(-3, 4, 6);
        scene.add(lime);
        const blue = new THREE.PointLight(0x38bdf8, 8, 14, 2);
        blue.position.set(5, -3, 3);
        scene.add(blue);

        function resize() {
            const width = host.clientWidth, height = host.clientHeight;
            if (!width || !height) return;
            camera.aspect = width / height;
            camera.updateProjectionMatrix();
            renderer.setSize(width, height, false);
        }
        const resizeObserver = new ResizeObserver(resize);
        resizeObserver.observe(host);
        resize();

        let pointer = 0, visible = true, frame;
        const visibilityObserver = new IntersectionObserver(([entry]) => { visible = entry.isIntersecting; });
        visibilityObserver.observe(host);
        host.parentElement.addEventListener('pointermove', (event) => {
            const rect = host.getBoundingClientRect();
            pointer = ((event.clientX - rect.left) / rect.width - 0.5) * 0.35;
        });

        function updateGeometry(time) {
            nodes.forEach((node, index) => {
                const wave = Math.sin(time * 0.00055 * node.speed + node.phase) * 0.16;
                const offset = index * 3;
                nodePositions[offset] = node.base.x + wave;
                nodePositions[offset + 1] = node.base.y + Math.cos(time * 0.00048 + node.phase) * 0.12;
                nodePositions[offset + 2] = node.base.z + wave * 0.7;
            });
            links.forEach(([from, to], index) => {
                const target = index * 6, a = from * 3, b = to * 3;
                linkPositions[target] = nodePositions[a];
                linkPositions[target + 1] = nodePositions[a + 1];
                linkPositions[target + 2] = nodePositions[a + 2];
                linkPositions[target + 3] = nodePositions[b];
                linkPositions[target + 4] = nodePositions[b + 1];
                linkPositions[target + 5] = nodePositions[b + 2];
            });
            for (let i = 0; i < packetCount; i++) {
                const [from, to] = links[(i * 7) % links.length];
                const start = from * 3, end = to * 3;
                const progress = (time * 0.00022 * (1 + (i % 3) * 0.18) + i / packetCount) % 1;
                const offset = i * 3;
                packetPositions[offset] = THREE.MathUtils.lerp(nodePositions[start], nodePositions[end], progress);
                packetPositions[offset + 1] = THREE.MathUtils.lerp(nodePositions[start + 1], nodePositions[end + 1], progress);
                packetPositions[offset + 2] = THREE.MathUtils.lerp(nodePositions[start + 2], nodePositions[end + 2], progress);
            }
            nodeGeometry.attributes.position.needsUpdate = true;
            linkGeometry.attributes.position.needsUpdate = true;
            packetGeometry.attributes.position.needsUpdate = true;
        }
        function render(time) {
            frame = requestAnimationFrame(render);
            if (!visible) return;
            updateGeometry(time);
            graph.rotation.y += (pointer - graph.rotation.y) * 0.015;
            graph.rotation.x = Math.sin(time * 0.00022) * 0.08;
            orbit.rotation.z = time * 0.00016;
            lime.intensity = 17 + Math.sin(time * 0.0014) * 3;
            packets.material.opacity = 0.72 + Math.sin(time * 0.002) * 0.24;
            renderer.render(scene, camera);
        }
        render(0);
        window.addEventListener('pagehide', () => {
            cancelAnimationFrame(frame);
            resizeObserver.disconnect();
            visibilityObserver.disconnect();
            renderer.dispose();
        }, { once: true });
    } catch (error) {
        console.warn('The decorative WebGL graph could not start.', error);
        host.remove();
    }
}
