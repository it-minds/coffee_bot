import {
  Box,
  Center,
  Heading,
  SimpleGrid,
  Spinner,
  Stat,
  StatArrow,
  StatGroup,
  StatHelpText,
  StatLabel,
  StatNumber,
  Text,
  useColorModeValue
} from "@chakra-ui/react";
import Timeline from "components/Timeline/Timeline";
import React, { FC } from "react";
import { useMemo } from "react";
import { ActiveRoundDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";
import { percentFormatter } from "utils/formatters/percentFormatter";

interface Props {
  round: ActiveRoundDto;
}

const RoundInfo: FC<Props> = ({ round }) => {
  if (!round) {
    return (
      <Center>
        <Spinner thickness="4px" speed="0.65s" emptyColor="gray.200" color="blue.500" size="xl" />
      </Center>
    );
  }
  const redColor = useColorModeValue("red.500", "red.700");
  const greenColor = useColorModeValue("green.500", "green.700");
  const statsBackground = useColorModeValue("blue.200", "blue.900");
  const imageBackdrop = useColorModeValue("rgb(255,255,255,0.6)", "rgb(0,0,0,0.4)");

  const stats = useMemo(() => {
    const meetup = round.groups.filter(x => x.hasMet).length / round.groups.length;
    const photo =
      round.groups.filter(x => x.hasPhoto).length /
      (round.groups.filter(x => x.hasMet).length || 1);

    return { meetup, photo };
  }, [round]);

  return (
    <>
      <Heading textAlign="center" fontSize={["2xl", "3xl", "4xl"]}>
        {dateTimeFormatter.format(round.startDate)} - {dateTimeFormatter.format(round.endDate)}
      </Heading>
      <Timeline round={round} />
      <StatGroup mt={6} mb={3}>
        <Stat backgroundColor={statsBackground} p={4}>
          <StatLabel>Meetup percent:</StatLabel>
          <StatNumber>{percentFormatter.format(stats.meetup * 100)}</StatNumber>
          <StatHelpText>
            <StatArrow type={stats.meetup > round.previousMeetup ? "increase" : "decrease"} />
            {percentFormatter.format(round.previousMeetup * 100)}
            {"  "}(previous)
          </StatHelpText>
        </Stat>

        <Stat backgroundColor={statsBackground} p={4}>
          <StatLabel>Photo percent</StatLabel>
          <StatNumber>{percentFormatter.format(stats.photo * 100)}</StatNumber>
          <StatHelpText>
            <StatArrow type={stats.photo > round.previousPhoto ? "increase" : "decrease"} />
            {percentFormatter.format(round.previousPhoto * 100)}
            {"  "}(previous)
          </StatHelpText>
        </Stat>
      </StatGroup>

      <Heading size="lg">Groups</Heading>
      <SimpleGrid
        spacingX={["0px", "15px", "30px"]}
        spacingY={["15px", "15px", "30px"]}
        mt={4}
        columns={[1, 2, 3, 4]}
        justifyItems="space-around">
        {round.groups.map((x, i) => (
          <Box
            key={x.id}
            backgroundColor={x.hasMet ? greenColor : redColor}
            backgroundImage={x.hasPhoto ? (x as any).publicSrc : ""}
            backgroundPosition="top"
            minH="250px"
            maxH="300px"
            minW="200px"
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
