import "ts-array-ext/sortByAttr";

import { Button, Select, Stack, StackDirection, useBreakpointValue } from "@chakra-ui/react";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import React from "react";
import { useState } from "react";
import { FC } from "react";
import { ChannelClient, ISimpleChannelDTO } from "services/backend/nswagts";

const NewChannelButton: FC = () => {
  const stackDirect = useBreakpointValue<StackDirection>({
    base: "column",
    md: "row"
  });

  const [availChannels, setAvailChannels] = useState<ISimpleChannelDTO[]>([]);

  const { genClient } = useNSwagClient(ChannelClient);

  useEffectAsync(async () => {
    const client = await genClient();
    const channels = await client.getMyAvailableChannels();

    setAvailChannels(
      channels.sort((a, b) => {
        if (a.isPrivate && !b.isPrivate) return 1;
        if (!a.isPrivate && b.isPrivate) return -1;

        if (a.name < b.name) return -1;
        if (a.name > b.name) return 1;
        return 0;
      })
    );
  }, []);

  return (
    <Stack direction={stackDirect}>
      <Select
        animation="slide-in"
        w={"100%"}
        marginInlineStart="0 !important"
        name="colors"
        placeholder="Select channel...">
        {availChannels.map(x => (
          <option value={x.id} key={x.id}>
            {/* <Icon as={x.isPrivate ? FaLock : FaHashtag} /> */}
            {x.isPrivate ? "(private) " : "(public) "}
            {x.name}
          </option>
        ))}
      </Select>
      <Button minW={64} onClick={() => null}>
        Create Buddy Channel
      </Button>
    </Stack>
  );
};

export default NewChannelButton;
