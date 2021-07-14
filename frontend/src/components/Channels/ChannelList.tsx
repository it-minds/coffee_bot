import {
  Center,
  Flex,
  Heading,
  Table,
  Tbody,
  Th,
  Thead,
  Tr,
  useBreakpointValue
} from "@chakra-ui/react";
import { ChannelContext } from "contexts/ChannelContext";
import React, { FC, useContext } from "react";
import { ChannelSettingsIdDto } from "services/backend/nswagts";

import ChannelListItem from "./ChannelListItem";

const ChannelList: FC = () => {
  const { channels } = useContext(ChannelContext);
  if (!channels) return null;

  const activePause = useBreakpointValue({
    base: "active",
    md: "pause/active"
  });

  return (
    <Center>
      <Flex direction="column">
        <Heading size="lg" textAlign="center">
          Channels you&#39;re a part of
        </Heading>
        <Table mt={2}>
          <Thead>
            <Tr>
              <Th pl={[1, 2, 4]} pr={[1, 2, 4]}>
                ChannelId
              </Th>
              <Th pl={[1, 2, 4]} pr={[1, 2, 4]}>
                {activePause}
              </Th>
              <Th pl={[1, 2, 4]} pr={[1, 2, 4]}></Th>
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
