import { Heading, HStack, Image, StackProps } from "@chakra-ui/react";
import React, { FC } from "react";

const Logo: FC<StackProps> = ({ color, ...props }) => {
  return (
    <HStack {...props}>
      <Image src="/images/icons/icon-128x128.png" w={8} />
      <Heading size="md" textAlign="left" color={color}>
        IT Minds
      </Heading>
    </HStack>
  );
};
export default Logo;
