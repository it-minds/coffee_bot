import {
  Box,
  Button,
  Flex,
  FormControl,
  FormLabel,
  NumberDecrementStepper,
  NumberIncrementStepper,
  NumberInput,
  NumberInputField,
  NumberInputStepper,
  Select,
  Switch
} from "@chakra-ui/react";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { FC, useCallback, useState } from "react";
import { ChannelClient, ChannelSettingsDto, DayOfWeek } from "services/backend/nswagts";

import HourPickerInput from "./HourPickerInput";

type Props = {
  submitCallback: (metaData: ChannelSettingsDto) => Promise<void>;
  channelId?: number;
};

const defaultChannel: ChannelSettingsDto = {
  groupSize: 2,
  startsDay: DayOfWeek.Thursday,
  weekRepeat: 1,
  durationInDays: 1,
  individualMessage: false,
  initializeRoundHour: 10,
  midwayRoundHour: 11,
  finalizeRoundHour: 16
};

const ChannelSettingsForm: FC<Props> = ({ submitCallback, channelId }) => {
  const [isLoading, setIsLoading] = useState(false);
  const [localFormData, setLocalFormData] = useState<ChannelSettingsDto>(defaultChannel);

  const { genClient } = useNSwagClient(ChannelClient);

  useEffectAsync(async () => {
    if (channelId) {
      setIsLoading(true);
      const client = await genClient();
      const channel = await client.getChannelSettings(channelId);

      setLocalFormData(channel);
      setIsLoading(false);
    }
  }, []);

  const onSubmit = useCallback(
    async event => {
      event.preventDefault();
      setIsLoading(true);
      await submitCallback(localFormData);
      setIsLoading(false);
    },
    [localFormData, submitCallback]
  );

  const updateLocalForm = useCallback((value: unknown, key: keyof ChannelSettingsDto) => {
    setLocalFormData(form => {
      (form[key] as unknown) = value;
      return { ...form };
    });
  }, []);

  return (
    <Flex w="full" align="center" justifyContent="center">
      <Box width="md" opacity={isLoading ? 0.2 : 1}>
        <form onSubmit={onSubmit}>
          <FormControl isRequired>
            <FormLabel>Start day</FormLabel>
            <Select
              onChange={event => updateLocalForm(event.target.value, "startsDay")}
              value={localFormData.startsDay}>
              <option value={DayOfWeek.Monday}>Monday</option>
              <option value={DayOfWeek.Tuesday}>Tuesday</option>
              <option value={DayOfWeek.Wednesday}>Wednesday</option>
              <option value={DayOfWeek.Thursday}>Thursday</option>
              <option value={DayOfWeek.Friday}>Friday</option>
              <option value={DayOfWeek.Saturday}>Saturday</option>
              <option value={DayOfWeek.Sunday}>Sunday</option>
            </Select>
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Group Size:</FormLabel>
            <NumberInput
              value={localFormData.groupSize}
              onChange={event => updateLocalForm(Number(event), "groupSize")}
              max={50}
              min={2}>
              <NumberInputField />
              <NumberInputStepper>
                <NumberIncrementStepper />
                <NumberDecrementStepper />
              </NumberInputStepper>
            </NumberInput>
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Duration in days:</FormLabel>
            <NumberInput
              value={localFormData.durationInDays}
              onChange={event => updateLocalForm(Number(event), "durationInDays")}
              max={50}
              min={1}>
              <NumberInputField />
              <NumberInputStepper>
                <NumberIncrementStepper />
                <NumberDecrementStepper />
              </NumberInputStepper>
            </NumberInput>
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Week Repeat:</FormLabel>
            <NumberInput
              value={localFormData.weekRepeat}
              onChange={event => updateLocalForm(Number(event), "weekRepeat")}
              max={20}
              min={1}>
              <NumberInputField />
              <NumberInputStepper>
                <NumberIncrementStepper />
                <NumberDecrementStepper />
              </NumberInputStepper>
            </NumberInput>
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Individual Message: </FormLabel>
            <Switch
              isChecked={localFormData.individualMessage}
              onChange={() =>
                updateLocalForm(!localFormData.individualMessage, "individualMessage")
              }
              size="lg"
            />
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Initialize Round Hour:</FormLabel>
            <HourPickerInput
              value={localFormData.initializeRoundHour}
              onChange={event =>
                updateLocalForm(Number(event.target.value.split(":")[0]), "initializeRoundHour")
              }
            />
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Midway Round Hour:</FormLabel>
            <HourPickerInput
              value={localFormData.midwayRoundHour}
              onChange={event =>
                updateLocalForm(Number(event.target.value.split(":")[0]), "midwayRoundHour")
              }
            />
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Finalize Round Hour:</FormLabel>
            <HourPickerInput
              value={localFormData.finalizeRoundHour}
              onChange={event =>
                updateLocalForm(Number(event.target.value.split(":")[0]), "finalizeRoundHour")
              }
            />
          </FormControl>
          <Button colorScheme="green" isLoading={isLoading} mt={6} type="submit">
            Submit
          </Button>
        </form>
      </Box>
    </Flex>
  );
};
export default ChannelSettingsForm;
