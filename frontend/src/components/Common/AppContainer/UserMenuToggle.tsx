import { Avatar, Box, HStack, MenuButton, Text } from "@chakra-ui/react";
import React, { FC } from "react";

const UserMenuToggle: FC = ({ children }) => {
  return (
    <Box as={MenuButton}>
      <HStack bg="blue.400" pl={2} borderRadius={32} cursor="pointer">
        <Text color="white">{children}</Text>
        <Avatar w={10} h={10} bg="pink.500" />
      </HStack>
    </Box>
  );
};

export default UserMenuToggle;
