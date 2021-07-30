import {
  Box,
  Container,
  Flex,
  Spacer,
  useBreakpointValue,
  useColorModeValue
} from "@chakra-ui/react";
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

  const topBarColor = useColorModeValue("blue.300", "blue.600");

  const user = useBreakpointValue({
    base: activeUser.email,
    sm: "User: " + activeUser.email
  });
  const version = useBreakpointValue({
    base: "v." + process.env.NEXT_PUBLIC_APP_VERSION,
    sm: "App Version: " + process.env.NEXT_PUBLIC_APP_VERSION
  });

  return (
    <BreadcrumbContext.Provider value={breadCrumbContextValue}>
      {/* <Center>
        <Stack> */}
      <Box w="100%" backgroundColor={topBarColor}>
        <Container p={2} maxW="6xl">
          <Flex align="center">
            <ColorModeToggler />
            <Breadcrumbs />
            <Spacer />
            <AppVersion>
              {user}
              <br />
              {version}
            </AppVersion>
          </Flex>
        </Container>
      </Box>
      <Container p={0} pt={[2, 4]} maxW="6xl">
        {children}
      </Container>
      {/* </Stack>
      </Center> */}
    </BreadcrumbContext.Provider>
  );
};

export default AppContainer;
