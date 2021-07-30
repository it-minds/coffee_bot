export function getBlobMeta(url: string, callback: (w: number, h: number) => void): void {
  const img = new Image();
  img.src = url;
  img.onload = function () {
    const thisImage = this as HTMLImageElement;
    callback(thisImage.width, thisImage.height);
  };
}

export async function getBlobMetaAsync(url: string): Promise<[w: number, h: number]> {
  return new Promise(resolve => {
    const img = new Image();
    img.src = url;
    img.onload = function () {
      const thisImage = this as HTMLImageElement;
      resolve([thisImage.width, thisImage.height]);
    };
  });
}
