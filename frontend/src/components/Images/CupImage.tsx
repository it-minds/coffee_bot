import { Image, ImageProps } from "@chakra-ui/react";
import { FC } from "react";

const CupImage: FC<Omit<ImageProps, "src" | "srcSet" | "alt">> = ({ ...props }) => {
  return (
    <Image
      src="/images/misc/cup-128x128.png"
      srcSet={`
        /images/misc/cup-128x128.png,
        /images/misc/cup-256x256.png 2x,
        /images/misc/cup-512x512.png 4x`}
      alt="Image of a prize cup"
      {...props}
    />
  );
};

export default CupImage;
