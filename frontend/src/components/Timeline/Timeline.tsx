import {
  Box,
  HStack,
  Slider,
  SliderFilledTrack,
  SliderThumb,
  SliderTrack,
  Spacer,
  Text,
  Tooltip,
  VStack
} from "@chakra-ui/react";
import { BsClock } from "@react-icons/all-files/bs/BsClock";
import useInterval from "hooks/useInterval";
import React, { FC, useState } from "react";
import { useCallback } from "react";
import { ActiveRoundDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";
import { percentFormatter } from "utils/formatters/percentFormatter";

interface Props {
  round: ActiveRoundDto;
}

const Timeline: FC<Props> = ({ round }) => {
  const getProgress = useCallback(() => {
    if (!round) return 0;

    const now = Date.now();

    const nowBaseStart = now - new Date(round.startDate).getTime();
    const endBaseStat = new Date(round.endDate).getTime() - new Date(round.startDate).getTime();

    return (nowBaseStart * 100) / endBaseStat;
  }, [round]);

  const [progress, setProgress] = useState<number>(getProgress);

  if (progress < 0 || progress > 100) return null;

  useInterval(() => {
    const progress = getProgress();
    console.debug("updating timeline", progress);
    setProgress(progress);
  }, 5000);

  return (
    <VStack textAlign="left">
      <Text mb={-7}>Current Progress:</Text>
      <HStack w="100%" marginBottom="-7px">
        <Text as="i">{dateTimeFormatter.format(new Date(round.startDate))}</Text>
        <Spacer />
        <Text as="i">{dateTimeFormatter.format(new Date(round.endDate))}</Text>
      </HStack>
      <Slider
        aria-label="slider-ex-1"
        isReadOnly
        value={progress}
        colorScheme="green"
        cursor="unset">
        <SliderTrack>
          <SliderFilledTrack />
        </SliderTrack>
        <Tooltip hasArrow label={percentFormatter.format(progress)} fontSize="md" placement="top">
          <SliderThumb boxSize={6} backgroundColor="green.100">
            <Box color="green.600" as={BsClock}></Box>
          </SliderThumb>
        </Tooltip>
      </Slider>
    </VStack>
  );
};

export default Timeline;
