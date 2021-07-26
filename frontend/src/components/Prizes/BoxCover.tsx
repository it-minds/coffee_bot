import { Box, useColorModeValue } from "@chakra-ui/react";
import React, { FC } from "react";

const BoxCover: FC = ({ children }) => {
  const bgColor = useColorModeValue("gray.100", "gray.700");

  return (
    <Box p={2} background={bgColor} borderRadius={4} w="100%" minH={24}>
      {children}
    </Box>
  );
};

export default BoxCover;
