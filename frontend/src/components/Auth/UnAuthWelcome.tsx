import {
  Box,
  Button,
  Container,
  Heading,
  Image,
  Stack,
  Text,
  useColorModeValue
} from "@chakra-ui/react";
import Logo from "components/Common/AppContainer/Logo";
import { AuthContext } from "contexts/AuthContext";
import { AuthStage, skipauth } from "hooks/useAuth";
import router from "next/router";
import React, { useCallback } from "react";
import { useContext } from "react";
import { FC } from "react";
import isomorphicEnvSettings from "utils/envSettings";
import { openSignInWindow } from "utils/openPopup";

import Footer from "./Footer";

const UnauthWelcome: FC = () => {
  const { authStage, login } = useContext(AuthContext);

  const click = useCallback(() => {
    const envSettings = isomorphicEnvSettings();

    if (authStage == AuthStage.UNAUTHENTICATED && router.pathname != skipauth) {
      openSignInWindow(envSettings.backendUrl + "/api/auth/login", "login", e => {
        if (typeof e.data == "string" && e.data.indexOf("?token=") === 0) {
          try {
            const pairs = (e.data as string).substring(1).split("&");

            let i: string;
            for (i in pairs) {
              if (!pairs[i] || pairs[i] === "") continue;

              const pair = pairs[i].split("=");
              const key = decodeURIComponent(pair[0]);
              const value = decodeURIComponent(pair[1]);

              if (key == "token") login(value);
            }
          } catch (e) {
            console.error(e, e.data);
          }
        }
      });
    }
  }, [authStage, login]);

  const bgColor = useColorModeValue("gray.200", "gray.700");
  const border = useColorModeValue("whiteAlpha.700", "whiteAlpha.300");

  return (
    <Container maxW="3xl">
      <Box p={8} mt={8} bgColor={bgColor} borderRadius={24}>
        <Logo mb={2} />
        <Heading textAlign="left" mb={2} fontWeight="extrabold">
          Welcome to Coffee Buddies
        </Heading>
        <Text fontSize="lg" color="gray.400" fontWeight="md" mb={4}>
          If your Slack Workspace already has the Coffee Buddies app, go ahead and login. Else you
          need to add the application to your Slack Workspace.
        </Text>
        <Image
          src="/images/icons/icon-512x512.png"
          srcSet={`
          /images/icons/icon-256x256.png 1x,
          /images/icons/icon-512x512.png 2x,
          /images/icons/icon-1024x1024.png 4x,
        `}
          mx="auto"
          my={4}
          maxW={"4xl"}
          w={["95%", "75%", "50%"]}
        />
        <Stack>
          <Button onClick={click} variant="outline" colorScheme="green">
            Login
          </Button>
          <Button
            borderColor={border}
            as="a"
            variant="outline"
            href="https://slack.com/oauth/v2/authorize?client_id=478982557798.1339171136881&scope=channels:read,chat:write,files:read,groups:read,groups:write,im:read,im:write,mpim:read,mpim:write,users.profile:read,users:read,app_mentions:read&user_scope=identity.basic,identity.email&redirect_uri=https://itm-coffee-bot-backend.azurewebsites.net/api/i/auth/app-install">
            <Image
              src="https://cdn.brandfolder.io/5H442O3W/at/pl546j-7le8zk-6gwiyo/Slack_Mark.svg"
              w={8}
            />
            Add App to Slack
          </Button>
        </Stack>
      </Box>
      <Footer />
    </Container>
  );
};
export default UnauthWelcome;
