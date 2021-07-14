import { Center, Container, Flex, Spacer, useBreakpointValue } from "@chakra-ui/react";
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

  const user = useBreakpointValue({
    base: activeUser.email,
    sm: "User: " + activeUser.email
  });
  const version = useBreakpointValue({
    base: "v." + process.env.NEXT_PUBLIC_APP_VERSION,
    sm: "App Version: " + process.env.NEXT_PUBLIC_APP_VERSION
  });

  return (
    <Center>
      <BreadcrumbContext.Provider value={breadCrumbContextValue}>
        <Container pt={[2, 4]} w="6xl" maxW="unset">
          <Flex justify="center" align="flex-start">
            <ColorModeToggler />
            <Breadcrumbs />
            <Spacer />
            <AppVersion>
              {user}
              <br />
              {version}
            </AppVersion>
          </Flex>
          {children}
        </Container>
      </BreadcrumbContext.Provider>
    </Center>
  );
};

export default AppContainer;
