import { Center, Container, Flex, Spacer, Text } from "@chakra-ui/react";
import ChannelList from "components/Channels/ChannelList";
import ColorModeToggler from "components/Common/ColorModeToggler";
import { AuthContext } from "contexts/AuthContext";
import { ChannelContext } from "contexts/ChannelContext";
import { useChannelContext } from "hooks/useChannelContext";
import { NextPage } from "next";
import { useContext } from "react";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const channelContext = useChannelContext();

  // return <Demo />;
  return (
    <ChannelContext.Provider value={channelContext}>
      <Center>
        <Container pt="20px" w="6xl" maxW="unset">
          <Flex justify="center" align="center">
            <ColorModeToggler />
            <Spacer />
            <Text>User: {activeUser.email}</Text>
          </Flex>
          <ChannelList />
        </Container>
      </Center>
    </ChannelContext.Provider>
  );
};

export default IndexPage;
