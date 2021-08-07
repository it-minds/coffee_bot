import { Box, BoxProps, useColorModeValue } from "@chakra-ui/react";
import React, { FC } from "react";

const RoundedBox: FC<BoxProps> = ({ children, ...rest }) => {
  return (
    <Box
      p={[4, 4, 8]}
      mt={[2, 4, 8]}
      mx={[0, 2, 4]}
      bgColor={useColorModeValue("gray.200", "gray.700")}
      borderRadius={24}
      {...rest}>
      {children}
    </Box>
  );
};

export default RoundedBox;
