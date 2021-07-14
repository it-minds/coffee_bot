import { Center, Container, Flex, Spacer, Tag, VStack } from "@chakra-ui/react";
import { BreadcrumbContext } from "components/Breadcrumbs/BreadcrumbContext";
import Breadcrumbs from "components/Breadcrumbs/Breadcrumbs";
import { useBreadcrumbsContext } from "components/Breadcrumbs/useBreadcrumbContext";
import ColorModeToggler from "components/Common/ColorModeToggler";
import { AuthContext } from "contexts/AuthContext";
import React, { FC, useContext } from "react";

import AppVersion from "./AppVersion";

const AppContainer: FC = ({ children }) => {
  const { activeUser } = useContext(AuthContext);

  const breadCrumbContextValue = useBreadcrumbsContext();

  return (
    <Center>
      <BreadcrumbContext.Provider value={breadCrumbContextValue}>
        <Container pt="20px" w="6xl" maxW="unset">
          <Flex justify="center" align="center">
            <ColorModeToggler />
            <Breadcrumbs />
            <Spacer />
            <VStack>
              <Tag>User: {activeUser.email}</Tag>
              <AppVersion />
            </VStack>
          </Flex>
          {children}
        </Container>
      </BreadcrumbContext.Provider>
    </Center>
  );
};

export default AppContainer;
