import {
  Box,
  Heading,
  HStack,
  Icon,
  SimpleGrid,
  Stat,
  StatArrow,
  StatGroup,
  StatHelpText,
  StatLabel,
  StatNumber,
  Text,
  useBreakpointValue,
  useColorModeValue
} from "@chakra-ui/react";
import { CgMathEqual } from "@react-icons/all-files/cg/CgMathEqual";
import { MdArrowBack } from "@react-icons/all-files/md/MdArrowBack";
import { MdArrowForward } from "@react-icons/all-files/md/MdArrowForward";
import PureIconsButton from "components/Common/PureIconButton";
import Timeline from "components/Timeline/Timeline";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useRouter } from "next/dist/client/router";
import React, { FC } from "react";
import { useMemo } from "react";
import { ActiveRoundDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";
import { percentFormatter } from "utils/formatters/percentFormatter";

interface Props {
  round: ActiveRoundDto;
}

const RoundInfo: FC<Props> = ({ round }) => {
  const router = useRouter();

  const redColor = useColorModeValue("red.500", "red.700");
  const greenColor = useColorModeValue("green.500", "green.700");
  const statsBackground = useColorModeValue("blue.200", "blue.900");
  const eqColor = useColorModeValue("yellow.400", "yellow.400");
  const imageBackdrop = useColorModeValue("rgb(255,255,255,0.6)", "rgb(0,0,0,0.4)");

  const stats = useMemo(() => {
    const meetup = (round.groups.filter(x => x.hasMet).length * 100) / round.groups.length;
    const photo =
      (round.groups.filter(x => x.hasPhoto).length * 100) /
      (round.groups.filter(x => x.hasMet).length || 1);

    return { meetup, photo };
  }, [round]);

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
      <HStack justifyContent="space-around">
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
        <Heading fontSize={["2xl", "3xl", "4xl"]}>
          {dateTimeFormatter.format(round.startDate)} - {dateTimeFormatter.format(round.endDate)}
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
      <StatGroup mt={6} mb={3} backgroundColor={statsBackground}>
        <Stat p={4}>
          <StatLabel>Meetup rate</StatLabel>
          <StatNumber>
            {stats.meetup == round.previousMeetup ? (
              <Icon w={3.5} color={eqColor} as={CgMathEqual} />
            ) : (
              <StatArrow type={stats.meetup > round.previousMeetup ? "increase" : "decrease"} />
            )}
            {percentFormatter.format(stats.meetup)}
          </StatNumber>
          <StatHelpText>previous: {percentFormatter.format(round.previousMeetup)}</StatHelpText>
        </Stat>

        <Stat p={4}>
          <StatLabel>Photo rate</StatLabel>
          <StatNumber>
            {stats.photo == round.previousPhoto ? (
              <Icon w={3.5} color={eqColor} as={CgMathEqual} />
            ) : (
              <StatArrow type={stats.photo > round.previousPhoto ? "increase" : "decrease"} />
            )}
            {percentFormatter.format(stats.photo)}
          </StatNumber>
          <StatHelpText>previous: {percentFormatter.format(round.previousPhoto)}</StatHelpText>
        </Stat>
      </StatGroup>

      <Heading size="lg">Groups</Heading>
      <SimpleGrid
        spacingX={[0, 2, 4]}
        spacingY={[1, 2, 4]}
        mt={4}
        columns={[1, 2, 3, 4]}
        justifyItems="space-around">
        {round.groups.map((x, i) => (
          <Box
            key={x.id}
            backgroundColor={x.hasMet ? greenColor : redColor}
            backgroundImage={x.hasPhoto ? (x as any).publicSrc : ""}
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
                {x.members.join(" & ")}
              </Text>
            </Box>
          </Box>
        ))}
      </SimpleGrid>
    </>
  );
};

export default RoundInfo;
