import { Center } from "@chakra-ui/react";
import UnauthWelcome from "components/Auth/UnAuthWelcome";
import AppContainer from "components/Common/AppContainer";
import OurSpinner from "components/Common/OurSpinner";
import { AuthContext } from "contexts/AuthContext";
import { AuthStage } from "hooks/useAuth";
import React, { useContext } from "react";
import { ComponentType, FC } from "react";

export const withAuth =
  (Component: ComponentType): FC =>
  // eslint-disable-next-line react/display-name
  ({ ...props }) => {
    const { authStage } = useContext(AuthContext);

    return authStage == AuthStage.UNAUTHENTICATED ? (
      <UnauthWelcome />
    ) : authStage == AuthStage.CHECKING ? (
      <Center mt={8}>
        <OurSpinner />
      </Center>
    ) : (
      <AppContainer>
        <Component {...props} />
      </AppContainer>
    );
  };
