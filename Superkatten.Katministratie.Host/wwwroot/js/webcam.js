function startVideo(src, preferredCameraDeviceId)
{
  if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia)
  {
    navigator
      .mediaDevices
      .getUserMedia({
        video: {
          width: 640,
          height: 480,
          deviceId: preferredCameraDeviceId
        }
      })
      .then(function (stream) {
        let video = document.getElementById(src);

        if ("srcObject" in video) {
          video.srcObject = stream;
        }
        else {
          video.src = window.URL.createObjectURL(stream);
        }

        video.onloadedmetadata = function (e) {
          video.play();
        };
      })
      .catch((err) => {
        console.error(`${err.name}: ${err.message}`);
      });
  }
}

function stopVideo() {

}

function getFrame(src, dest, dotNetHelper)
{
  let video = document.getElementById(src);
  let canvas = document.getElementById(dest);

  canvas
    .getContext('2d')
    .drawImage(video, 0, 0, 640, 480);

  let dataUrl = canvas.toDataURL("image/jpeg");
  dotNetHelper.invokeMethodAsync('ProcessImage', dataUrl);
}

function getCameraDeviceList()
{
  return navigator
    .mediaDevices
    .enumerateDevices()
    .then(c => c.filter(o => o.kind === "videoinput"));
}