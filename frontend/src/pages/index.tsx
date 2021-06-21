import { Center, Container, Flex, Heading, Spacer, Text } from "@chakra-ui/react";
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
        <Flex justify="center" align="center">
          <ColorModeToggler />
          <Spacer />
          <Text> User: {activeUser.email}</Text>
        </Flex>
        <ChannelList />
      </Container>
    </Center>
  );
};

export default IndexPage;
