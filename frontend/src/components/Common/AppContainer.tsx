import { Box, Container, Flex, Spacer } from "@chakra-ui/react";
import ChannelActionsMenu from "components/Channels/ChannelActionsMenu";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import { useRouter } from "next/dist/client/router";
import React, { FC, useMemo } from "react";

import Logo from "./AppContainer/Logo";
import UserMenu from "./AppContainer/UserMenu";

const AppContainer: FC = ({ children }) => {
  const { push, query } = useRouter();

  const channelId = useMemo(() => {
    if (!query.channelId) return null;
    const channelId = parseInt(query.channelId as string);
    return channelId;
  }, [query]);

  return (
    <ChosenChannelContext.Provider
      value={{
        chosenChannel: {
          id: channelId,
          name: ""
        }
      }}>
      <Box w="100%" backgroundColor="blue.600">
        <Container p={2} maxW="6xl">
          <Flex align="center" justify="space-between">
            <Logo
              onClick={() => push("/")}
              cursor="pointer"
              _hover={{
                textDecoration: "underline"
              }}
              color="white"
            />
            {channelId && (
              <Box ml={8}>
                <ChannelActionsMenu channelId={channelId} />
              </Box>
            )}
            <Spacer />
            <UserMenu />
          </Flex>
        </Container>
      </Box>
      <Container p={0} pt={[2, 4]} maxW="6xl">
        {children}
      </Container>
    </ChosenChannelContext.Provider>
  );
};

export default AppContainer;
