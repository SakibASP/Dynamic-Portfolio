document.getElementById('fileInput').addEventListener('change', function (event) {
    const file = event.target.files[0];
    const previewDiv = document.getElementById('file-preview');
    previewDiv.innerHTML = '';

    if (file.type === 'application/pdf') {
        try {
            const pdfEmbed = document.createElement('embed');
            pdfEmbed.src = URL.createObjectURL(file);
            pdfEmbed.type = 'application/pdf';
            pdfEmbed.width = '150px';
            pdfEmbed.height = '150px';
            previewDiv.appendChild(pdfEmbed);
        }
        catch (err) {
            console.error(err);
        }
    }
    else if (file.type === 'application/zip' || file.type === 'application/x-zip-compressed') {
        try {
            JSZip.loadAsync(file).then(function (zip) {
                const ul = document.createElement('ul');
                Object.keys(zip.files).forEach(function (filename) {
                    const li = document.createElement('li');
                    li.textContent = filename;
                    ul.appendChild(li);
                });
                previewDiv.appendChild(ul);
            });
        }
        catch (err) {
            console.error(err);
        }
    }
    else if (file.type.startsWith('image/')) {
        try {
            const reader = new FileReader();
            reader.onload = function (e) {
                const img = document.createElement('img');
                img.src = e.target.result;
                img.style.maxWidth = '150px';
                img.style.height = '150px';
                previewDiv.appendChild(img);
            };
            reader.readAsDataURL(file);
        }
        catch (err) {
            console.error(err);
        }
    }
});