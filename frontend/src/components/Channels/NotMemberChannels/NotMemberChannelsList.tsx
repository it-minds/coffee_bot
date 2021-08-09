import { Box, Button, HStack, Icon, Spacer, Text } from "@chakra-ui/react";
import { HiHashtag } from "@react-icons/all-files/hi/HiHashtag";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { useState } from "react";
import { FC } from "react";
import { ChannelClient, ChannelMemberDTO } from "services/backend/nswagts";

const NotMemberChannelsList: FC = () => {
  const [channels, setChannels] = useState<ChannelMemberDTO[]>([]);

  const { genClient } = useNSwagClient(ChannelClient);

  useEffectAsync(async () => {
    const client = await genClient();
    const result = await client.getMyNotChannelMemberships();
    setChannels(result);
  }, []);

  return (
    <Box>
      {channels.length === 0 && "There aren't any more channels for you to join."}
      {channels.map(channel => (
        <HStack key={channel.id}>
          <Text>
            <Icon as={HiHashtag} color="orange.500" mb={0.25} />
            {channel.channelSettings.slackChannelName}
          </Text>
          <Spacer />
          <Button>Click to join</Button>
        </HStack>
      ))}
    </Box>
  );
};

export default NotMemberChannelsList;
