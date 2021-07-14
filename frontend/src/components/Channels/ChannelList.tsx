import { Center, Flex, Heading, Table, Tbody, Th, Thead, Tr } from "@chakra-ui/react";
import { ChannelContext } from "contexts/ChannelContext";
import React, { FC, useContext } from "react";
import { ChannelSettingsIdDto } from "services/backend/nswagts";

import ChannelListItem from "./ChannelListItem";

const ChannelList: FC = () => {
  const { channels } = useContext(ChannelContext);

  if (!channels) return null;
  return (
    <Center>
      <Flex direction="column">
        <Heading size="lg" textAlign="center">
          Channels you&#39;re a part of
        </Heading>
        <Table mt={2}>
          <Thead>
            <Tr>
              <Th>ChannelId</Th>
              <Th isNumeric>Pause/Active</Th>
              <Th isNumeric>Actions</Th>
            </Tr>
          </Thead>
          <Tbody>
            {channels.map((channel: ChannelSettingsIdDto) => (
              <ChannelListItem key={channel.id} channel={channel} />
            ))}
          </Tbody>
        </Table>
      </Flex>
    </Center>
  );
};

export default ChannelList;
