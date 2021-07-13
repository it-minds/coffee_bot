import { Container, Heading } from "@chakra-ui/react";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useState } from "react";
import { genChannelClient } from "services/backend/apiClients";
import { ActiveRoundDto } from "services/backend/nswagts";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const { query } = useRouter();

  const [round, setRound] = useState<ActiveRoundDto>(null);

  useEffectAsync(async () => {
    if (activeUser && query.channelId) {
      const channelId = parseInt(query.channelId as string);

      const client = await genChannelClient();
      const result = await client.getActiveRound(channelId);

      setRound(result);
    }
  }, [activeUser, query]);

  return (
    <Container maxW="7xl">
      <Heading textAlign="center">Active Round</Heading>
      {round && <pre>{JSON.stringify(round, null, 2)}</pre>}
    </Container>
  );
};

export default IndexPage;
