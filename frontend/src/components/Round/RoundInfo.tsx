import "ts-array-ext/sum";

import {
  Box,
  Heading,
  HStack,
  SimpleGrid,
  Text,
  useBreakpointValue,
  useColorModeValue
} from "@chakra-ui/react";
import { MdArrowBack } from "@react-icons/all-files/md/MdArrowBack";
import { MdArrowForward } from "@react-icons/all-files/md/MdArrowForward";
import { DividerWithText } from "components/Common/DividerWIthText";
import PureIconsButton from "components/Common/PureIconButton";
import RoundedBox from "components/Common/RoundedBox";
import Timeline from "components/Timeline/Timeline";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useRouter } from "next/dist/client/router";
import React, { FC } from "react";
import { ActiveRoundDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";

import StatsBox from "./StatsBox";
import { generateRoundStats } from "./utils/generateRoundStats";

interface Props {
  round: ActiveRoundDto;
}

const RoundInfo: FC<Props> = ({ round }) => {
  const router = useRouter();

  const redColor = useColorModeValue("red.500", "red.700");
  const greenColor = useColorModeValue("green.500", "green.700");
  const imageBackdrop = useColorModeValue("rgb(255,255,255,0.6)", "rgb(0,0,0,0.4)");

  useEffectAsync(async () => {
    if (round.nextId != null)
      await router.prefetch(
        "/channels/[channelId]/rounds/[roundId]",
        `/channels/${router.query.channelId}/rounds/${round.nextId}`
      );
    if (round.previousId != null)
      await router.prefetch(
        "/channels/[channelId]/rounds/[roundId]",
        `/channels/${router.query.channelId}/rounds/${round.previousId}`
      );
  }, []);

  return (
    <>
      <Heading textAlign="center" fontSize={["xl", "2xl", "3xl"]}>
        #mychannelname
      </Heading>
      <RoundedBox>
        <HStack justifyContent="space-between">
          <PureIconsButton
            icon={MdArrowBack}
            onClick={() =>
              router.push(
                "/channels/[channelId]/rounds/[roundId]",
                `/channels/${router.query.channelId}/rounds/${round.previousId}`
              )
            }
            isDisable={round.previousId == null}
          />
          <Heading fontSize={["lg", "xl"]}>
            {dateTimeFormatter.format(new Date(round.startDate))} -{" "}
            {dateTimeFormatter.format(new Date(round.endDate))}
          </Heading>
          <PureIconsButton
            icon={MdArrowForward}
            onClick={() =>
              router.push(
                "/channels/[channelId]/rounds/[roundId]",
                `/channels/${router.query.channelId}/rounds/${round.nextId}`
              )
            }
            isDisable={round.nextId == null}
          />
        </HStack>
        <Timeline round={round} />

        <StatsBox pairs={generateRoundStats(round)} />
        <DividerWithText>
          <Heading size="md">Groups</Heading>
        </DividerWithText>
        <SimpleGrid
          spacingX={[0, 2, 4]}
          spacingY={[1, 2, 4]}
          mt={4}
          columns={[1, 2, 3, 4]}
          justifyItems="space-around">
          {round.coffeeRoundGroups.map((x, i) => (
            <Box
              key={x.id}
              backgroundColor={x.hasMet ? greenColor : redColor}
              backgroundImage={x.hasPhoto ? x.localPhotoUrl : ""}
              backgroundPosition={useBreakpointValue({
                base: "center",
                md: "top"
              })}
              backgroundSize="cover"
              minH={64}
              maxH={70}
              minW={64}
              p={[2, 3, 4]}>
              {/* <pre>{JSON.stringify(x, null, 2)}</pre> */}
              <Box backgroundColor={imageBackdrop} p={[1, 2]}>
                <Heading size="md" textAlign="center">
                  Group {i + 1}
                </Heading>
                <Text textAlign="center" fontSize="xs">
                  {x.coffeeRoundGroupMembers
                    .map(y => y.slackMemberName || y.slackMemberId)
                    .oxfordJoin(", ", ", and ")}
                </Text>
              </Box>
            </Box>
          ))}
        </SimpleGrid>
      </RoundedBox>
    </>
  );
};

export default RoundInfo;

declare global {
  interface Array<T> {
    oxfordJoin(separator: string, lastPlaceSeparator: string): string;
  }
}

Array.prototype.oxfordJoin = function (separator: string, lastPlaceSeparator: string) {
  const lastElement = (this as any[]).splice(this.length - 1, 1);

  return this.join(separator) + lastPlaceSeparator + lastElement;
};
