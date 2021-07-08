export function getBlobMeta(url: string, callback: (w: number, h: number) => void) {
  const img = new Image();
  img.src = url;
  img.onload = function () {
    callback((this as HTMLImageElement).width, (this as HTMLImageElement).height);
  };
}
