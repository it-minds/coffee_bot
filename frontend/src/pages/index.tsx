import { Center, Container, Flex, Text } from "@chakra-ui/react";
import ChannelList from "components/Channels/ChannelList";
import ColorModeToggler from "components/Common/ColorModeToggler";
import { AuthContext } from "contexts/AuthContext";
import { NextPage } from "next";
import { useContext } from "react";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);

  // return <Demo />;
  return (
    <Center>
      <Container pt="20px" w="6xl" maxW="unset">
        <Flex>
          <ColorModeToggler />
          <Text> User: {activeUser.email}</Text>
        </Flex>
        <ChannelList />
      </Container>
    </Center>
  );
};

export default IndexPage;
