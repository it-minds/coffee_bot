import RoundInfo from "components/Round/RoundInfo";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useState } from "react";
import { genChannelClient } from "services/backend/apiClients";
import { ActiveRoundDto } from "services/backend/nswagts";
import isomorphicEnvSettings from "utils/envSettings";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const { query } = useRouter();

  const [round, setRound] = useState<ActiveRoundDto>(null);

  useEffectAsync(async () => {
    if (activeUser && query.channelId) {
      const channelId = parseInt(query.channelId as string);

      const client = await genChannelClient();
      const result = await client.getActiveRound(channelId);

      const envSettings = isomorphicEnvSettings();

      result.groups.forEach(group => {
        (group as any).publicSrc =
          envSettings.backendUrl + "/images/coffeegroups/" + group.photoUrl;
      });

      setRound(result);
    }
  }, [activeUser, query]);

  return <RoundInfo round={round} />;
};

export default IndexPage;
