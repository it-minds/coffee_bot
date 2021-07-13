import { Center, Container, Flex, Spacer, Tag } from "@chakra-ui/react";
import ColorModeToggler from "components/Common/ColorModeToggler";
import { AuthContext } from "contexts/AuthContext";
import React, { FC, useContext } from "react";

const AppContainer: FC = ({ children }) => {
  const { activeUser } = useContext(AuthContext);

  return (
    <Center>
      <Container pt="20px" w="6xl" maxW="unset">
        <Flex justify="center" align="center">
          <ColorModeToggler />
          <Spacer />
          <Tag>User: {activeUser.email}</Tag>
        </Flex>
        {children}
      </Container>
    </Center>
  );
};

export default AppContainer;
