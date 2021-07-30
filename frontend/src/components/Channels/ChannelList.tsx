import {
  Box,
  Container,
  Flex,
  Heading,
  List,
  ListIcon,
  ListItem,
  Text,
  useBreakpointValue,
  useColorModeValue
} from "@chakra-ui/react";
import { BiShield } from "@react-icons/all-files/bi/BiShield";
import { DividerWithText } from "components/Common/DividerWIthText";
import { ChannelContext } from "contexts/ChannelContext";
import React, { FC, useContext } from "react";
import { ChannelSettingsIdDto } from "services/backend/nswagts";

import ChannelListItem from "./ChannelListItem";
import NewChannelButton from "./NewChannelButton";

const ChannelList: FC = () => {
  const { channels } = useContext(ChannelContext);
  if (!channels) return null;

  const activePause = useBreakpointValue({
    base: "active",
    md: "pause/active"
  });

  const bgColor = useColorModeValue("gray.200", "gray.700");
  const border = useColorModeValue("whiteAlpha.700", "whiteAlpha.300");

  return (
    <Container maxW="4xl">
      <Heading size="lg" textAlign="center">
        Buddy Channels!
      </Heading>
      <Box p={8} mt={8} bgColor={bgColor} borderRadius={24}>
        <Text>
          Welcome to your Coffee Buddies home away from home. Here you will find the channels
          available for you to join or should you be daring type where you get to create a new one!
          <br />
          Once you are in, your active channels hold:
          <List>
            <ListItem>
              <ListIcon color="blue.500" as={BiShield} />
              an overview of all the rounds present and past.
            </ListItem>
            <ListItem>
              <ListIcon color="blue.500" as={BiShield} />a gallery of all the important beautiful
              selfies everyone has shared.
            </ListItem>
            <ListItem>
              <ListIcon color="blue.500" as={BiShield} />
              and most importantly, the prizes!
            </ListItem>
          </List>
          <br />
        </Text>
        <DividerWithText m={1}>Your memberships</DividerWithText>
        <Flex direction="column">
          {channels.map((channel: ChannelSettingsIdDto) => (
            <ChannelListItem key={channel.id} channel={channel} />
          ))}
        </Flex>
        <DividerWithText m={1}>Other open channels in your organization</DividerWithText>
        <Flex direction="column">
          {/* {channels.map((channel: ChannelSettingsIdDto) => (
            <ChannelListItem key={channel.id} channel={channel} />
          ))} */}
          <Text as="i">Not yet implemented</Text>
        </Flex>
        <DividerWithText m={1}>Create a new buddies channel</DividerWithText>
        <Text mb={4}>
          Before you create a new buddy channel, you must first create the actual Slack channel. You
          can choose between private and public, but if your channel is private users will not see
          it on the list above of available channels they can join.
          <br />
          Once the channel has been created, you will need to select an admin. The admin can change
          settings and set up the prizes.
        </Text>
        <NewChannelButton />
      </Box>
    </Container>
  );
};

export default ChannelList;
