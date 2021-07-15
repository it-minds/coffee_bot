import { Center, Spinner } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import RoundInfo from "components/Round/RoundInfo";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNswagClient";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useState } from "react";
import { ActiveRoundDto, IRoundClient, RoundClient } from "services/backend/nswagts";
import isomorphicEnvSettings from "utils/envSettings";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const { query } = useRouter();

  useBreadcrumbs(
    [
      {
        name: "home",
        path: "/"
      },
      {
        name: "channel " + query.channelId,
        path: "/channels/[channelId]/rounds",
        asPath: `/channels/${query.channelId}/rounds`
      },
      {
        name: "rounds",
        path: "/channels/[channelId]/rounds",
        asPath: `/channels/${query.channelId}/rounds`
      },
      {
        name: query.roundId as string,
        path: "/channels/[channelId]/rounds/[roundId]",
        asPath: `/channels/${query.channelId}/rounds/${query.roundId}`
      }
    ],
    [query]
  );

  const [round, setRound] = useState<ActiveRoundDto>(null);

  const { genClient } = useNSwagClient<IRoundClient>(RoundClient);

  useEffectAsync(async () => {
    if (!activeUser || !query.roundId) return;

    setRound(null);

    const roundId = parseInt(query.roundId as string);

    const client = await genClient();
    const result = await client.getRound(roundId);

    const envSettings = isomorphicEnvSettings();

    result.groups.forEach(group => {
      (group as any).publicSrc = envSettings.backendUrl + "/images/coffeegroups/" + group.photoUrl;
    });

    setRound(result);
  }, [activeUser, query]);

  return (
    <>
      {round ? (
        <RoundInfo round={round} />
      ) : (
        <Center>
          <Spinner thickness="4px" speed="0.65s" emptyColor="gray.200" color="blue.500" size="xl" />
        </Center>
      )}
    </>
  );
};

export default IndexPage;
