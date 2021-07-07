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
import React, { FC, useCallback, useEffect, useState } from "react";
import { DayOfWeek, IChannelSettingsDto } from "services/backend/nswagts";

type Props = {
  submitCallback: (metaData: IChannelSettingsDto) => Promise<void>;
  channel?: IChannelSettingsDto;
};

const ChannelSettingsForm: FC<Props> = ({ submitCallback, channel }) => {
  const [isLoading, setIsLoading] = useState(false);
  const [localFormData, setLocalFormData] = useState<IChannelSettingsDto>({
    groupSize: 2,
    startsDay: DayOfWeek.Monday,
    weekRepeat: 1,
    durationInDays: 1,
    individualMessage: false
  });
  useEffect(() => {
    if (channel) {
      setLocalFormData(channel);
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

  const updateLocalForm = useCallback((value: unknown, key: keyof IChannelSettingsDto) => {
    setLocalFormData(form => {
      (form[key] as unknown) = value;
      return { ...form };
    });
  }, []);

  return (
    <Flex w="full" align="center" justifyContent="center">
      <Box width="md">
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
          <Button colorScheme="blue" isLoading={isLoading} width="100px" mt={6} type="submit">
            Submit
          </Button>
        </form>
      </Box>
    </Flex>
  );
};
export default ChannelSettingsForm;
