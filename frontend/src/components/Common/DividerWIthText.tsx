import { Box, Divider, Flex, FlexProps, Text, useColorModeValue } from "@chakra-ui/react";
import React, { FC } from "react";

export const DividerWithText: FC<FlexProps> = props => {
  const { children, ...flexProps } = props;
  return (
    <Flex align="center" color="gray.300" {...flexProps}>
      <Box flex="1" minW={1}>
        <Divider borderColor="currentcolor" />
      </Box>
      <Text
        as="span"
        px="1"
        color={useColorModeValue("gray.600", "gray.400")}
        fontWeight="semibold"
        bg={useColorModeValue("blackAlpha.50", "whiteAlpha.50")}
        borderRadius={2}>
        {children}
      </Text>
      <Box flex="1" minW={1}>
        <Divider borderColor="currentcolor" />
      </Box>
    </Flex>
  );
};
