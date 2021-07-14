import {
  Box,
  HStack,
  Slider,
  SliderFilledTrack,
  SliderThumb,
  SliderTrack,
  Spacer,
  Text,
  VStack
} from "@chakra-ui/react";
import { BsClock } from "@react-icons/all-files/bs/BsClock";
import React, { FC, useMemo } from "react";
import { ActiveRoundDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";

interface Props {
  round: ActiveRoundDto;
}

const Timeline: FC<Props> = ({ round }) => {
  const progress = useMemo(() => {
    if (!round) return 0;

    const now = Date.now();

    const nowBaseStart = now - round.startDate.getTime();
    const endBaseStat = round.endDate.getTime() - round.startDate.getTime();

    return (nowBaseStart * 100) / endBaseStat;
  }, [round]);

  if (progress < 0 || progress > 100) return null;

  return (
    <VStack textAlign="left">
      <Text mb={-7}>Current Progress:</Text>
      <HStack w="100%" marginBottom="-7px">
        <Text as="i">{dateTimeFormatter.format(round.startDate)}</Text>
        <Spacer />
        <Text as="i">{dateTimeFormatter.format(round.endDate)}</Text>
      </HStack>
      <Slider
        aria-label="slider-ex-1"
        isReadOnly
        defaultValue={progress}
        colorScheme="green"
        cursor="unset">
        <SliderTrack>
          <SliderFilledTrack />
        </SliderTrack>
        <SliderThumb boxSize={6} backgroundColor="green.100">
          <Box color="green.600" as={BsClock}></Box>
        </SliderThumb>
      </Slider>
    </VStack>
  );
};

export default Timeline;
