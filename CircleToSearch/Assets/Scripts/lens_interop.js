// This script will be injected into WebView2 to interact with the Google Lens page.
async function uploadImageFromBase64(base64Data) {
    try {
        const response = await fetch(base64Data);
        const blob = await response.blob();

        const imageFile = new File([blob], 'captured_image.png', { type: 'image/png' });

        const dataTransfer = new DataTransfer();
        dataTransfer.items.add(imageFile);

        // Google Lens�� ���� �Է� ��Ҹ� ã���ϴ�. �� �����ڴ� ����� �� �ֽ��ϴ�.
        const fileInput = document.querySelector('input[type="file"]');

        if (fileInput) {
            fileInput.files = dataTransfer.files;
            const event = new Event('change', { bubbles: true });
            fileInput.dispatchEvent(event);
        } else {
            console.error('File input element not found.');
        }
    } catch (error) {
        console.error('Error uploading image:', error);
    }
}