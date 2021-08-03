import { Box, Button } from "@chakra-ui/react";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { useContext, useState } from "react";
import { useCallback } from "react";
import { FC } from "react";
import { ClaimedUserPrizeDTO, PrizesClient } from "services/backend/nswagts";

const PrizeClaimsList: FC = () => {
  const { chosenChannel } = useContext(ChosenChannelContext);

  const { genClient } = useNSwagClient(PrizesClient);

  const [claims, setClaims] = useState<ClaimedUserPrizeDTO[]>([]);

  useEffectAsync(async () => {
    const client = await genClient();
    const result = await client.getClaimedPrizesForApproval(chosenChannel.id);
    setClaims(result);
  }, []);

  const claimPrize = useCallback(async (id: number) => {
    const client = await genClient();
    await client.deliverClaimedPrize({
      claimPrizeId: id
    });
  }, []);

  return (
    <Box>
      {claims.map(x => (
        <Box key={x.id} p={2} m={2} bg="gray.500">
          <b>{x.channelMember.slackName}</b> claimed <b>{x.prize.title}</b>{" "}
          {x.prize.isMilestone && "(milestone)"}
          {x.prize.isRepeatable && "(repeatable)"}
          <br />
          {x.dateClaimed.toDateString()}
          <br />
          <Button onClick={() => claimPrize(x.id)}>Set as delivered</Button>
        </Box>
      ))}
    </Box>
  );
};

export default PrizeClaimsList;
