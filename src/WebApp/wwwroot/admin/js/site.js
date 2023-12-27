ClassicEditor
    .create(document.querySelector('#_description'))
    .then(editor => {
        editor.model.document.on('change', () => {
            document.getElementById('Description').value = editor.getData();
        });
    })
    .catch(error => {
        console.error(error);
    });

CKEDITOR.replace('_description', {
    width: '70%',
    height: 500,
    removeButtons: 'PasteFromWord'
});

function displaySelectedImage(event, elementId) {
    const selectedImage = document.getElementById(elementId);
    const fileInput = event.target;

    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();

        reader.onload = function (e) {
            selectedImage.src = e.target.result;
        };

        reader.readAsDataURL(fileInput.files[0]);
    }
}
