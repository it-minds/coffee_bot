import {
  Box,
  Button,
  Container,
  Heading,
  HStack,
  Image,
  List,
  ListIcon,
  ListItem,
  Stack,
  Text,
  useColorModeValue
} from "@chakra-ui/react";
import { VscCircleFilled } from "@react-icons/all-files/vsc/VscCircleFilled";
import Footer from "components/Auth/Footer";
import { DividerWithText } from "components/Common/DividerWIthText";
import { NextPage } from "next";
import React from "react";

const IndexPage: NextPage = () => {
  const bgColor = useColorModeValue("gray.200", "gray.700");
  const border = useColorModeValue("whiteAlpha.700", "whiteAlpha.300");

  return (
    <Container maxW="3xl">
      <Box p={8} mt={8} bgColor={bgColor} borderRadius={24}>
        <HStack mb={2}>
          <Image src="/images/icons/icon-128x128.png" w={8} />
          <Heading size="md" textAlign="left">
            IT Minds
          </Heading>
        </HStack>
        <Heading textAlign="left" mb={2} fontWeight="extrabold">
          Information about Coffee Buddies
        </Heading>
        <Text fontSize="lg" color="gray.400" fontWeight="md" mb={4}>
          Coffee Buddies is an option for your community / workspace to get to know each other by
          having coffee dates arranged automatically between all participants.
          <DividerWithText>How</DividerWithText>
          The way it works is by synchronizing all members of the channel the @coffee-bot has been
          added to. Then every 2 weeks (adjustable) all channel members are split into arbitrary
          groups of 3 people (adjustable). Over the course of the rounds duration the groups have to
          meet up for a coffee date, socialize and most importantly take a group selfie. The groups
          report in in the channel and share their selfie.
          <br />
          The round ends as a new begins.
          <DividerWithText>Points</DividerWithText>
          Each channel can setup a prizing and point system to award members for participation.
          Prizes are divided into three categories.
          <List>
            {["Milestones", "Repeatable", "One-Time"].map(x => (
              <ListItem key={"blt-" + x}>
                <ListIcon as={VscCircleFilled} color="blue.500" />
                {x}
              </ListItem>
            ))}
          </List>
          The members can use this site to view and claim their prizes.
        </Text>
        <Stack>
          <Button
            width="100%"
            borderColor={border}
            as="a"
            variant="outline"
            href="https://slack.com/oauth/v2/authorize?client_id=478982557798.1339171136881&scope=channels:read,chat:write,files:read,groups:read,groups:write,im:read,im:write,mpim:read,mpim:write,users.profile:read,users:read,app_mentions:read&user_scope=identity.basic,identity.email&redirect_uri=https://itm-coffee-bot-backend.azurewebsites.net/api/i/auth/app-install">
            <Image
              src="https://cdn.brandfolder.io/5H442O3W/at/pl546j-7le8zk-6gwiyo/Slack_Mark.svg"
              w={8}
            />
            Add Coffee Buddies to Slack
          </Button>
        </Stack>
      </Box>
      <Footer />
    </Container>
  );
};

export default IndexPage;
