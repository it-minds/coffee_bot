import {
  Box,
  Center,
  Container,
  Heading,
  Spinner,
  Stat,
  StatArrow,
  StatGroup,
  StatHelpText,
  StatLabel,
  StatNumber,
  Text,
  useColorModeValue,
  Wrap
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

  const stats = useMemo(() => {
    const meetup = round.groups.filter(x => x.hasMet).length / round.groups.length;
    const photo =
      round.groups.filter(x => x.hasPhoto).length /
      (round.groups.filter(x => x.hasMet).length || 1);

    return { meetup, photo };
  }, [round]);

  return (
    <Container maxW="7xl">
      <Heading textAlign="center">
        Round {dateTimeFormatter.format(round.startDate)} -{" "}
        {dateTimeFormatter.format(round.endDate)}
      </Heading>
      <Timeline round={round} />
      <StatGroup mt={6} mb={3}>
        <Stat backgroundColor="blue.900" p={4}>
          <StatLabel>Meetup percent:</StatLabel>
          <StatNumber>{percentFormatter.format(stats.meetup * 100)}</StatNumber>
          <StatHelpText>
            <StatArrow type={stats.meetup > round.previousMeetup ? "increase" : "decrease"} />
            {percentFormatter.format(round.previousMeetup * 100)}
          </StatHelpText>
        </Stat>

        <Stat backgroundColor="blue.900" p={4}>
          <StatLabel>Photo percent</StatLabel>
          <StatNumber>{percentFormatter.format(stats.photo * 100)}</StatNumber>
          <StatHelpText>
            <StatArrow type={stats.photo > round.previousPhoto ? "increase" : "decrease"} />
            {percentFormatter.format(round.previousPhoto * 100)}
          </StatHelpText>
        </Stat>
      </StatGroup>

      <Heading size="lg">Groups</Heading>
      <Wrap spacing={[2, 6]} mt={4}>
        {round.groups.map((x, i) => (
          <Box
            key={x.id}
            backgroundColor={x.hasMet ? greenColor : redColor}
            backgroundImage={x.hasPhoto ? (x as any).publicSrc : ""}
            backgroundPosition="top"
            h="300px"
            w="300px"
            p={[2, 3, 4]}>
            {/* <pre>{JSON.stringify(x, null, 2)}</pre> */}
            <Heading size="md" textAlign="center">
              Group {i + 1}
            </Heading>
            <Text textAlign="center">{x.members.join(", ")}</Text>
          </Box>
        ))}
      </Wrap>
    </Container>
  );
};

export default RoundInfo;
