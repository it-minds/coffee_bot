import {
  HStack,
  Slider,
  SliderFilledTrack,
  SliderThumb,
  SliderTrack,
  Spacer,
  Text,
  VStack
} from "@chakra-ui/react";
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

    return nowBaseStart / endBaseStat;
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
        defaultValue={progress * 100}
        colorScheme="yellow"
        sx={{
          cursor: "unset"
        }}>
        <SliderTrack>
          <SliderFilledTrack />
        </SliderTrack>
        <SliderThumb />
      </Slider>
    </VStack>
  );
};

export default Timeline;
