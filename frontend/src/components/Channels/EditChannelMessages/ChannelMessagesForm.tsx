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
  Textarea,
  Switch
} from "@chakra-ui/react";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { FC, useCallback, useState } from "react";
import { ChannelClient, ChannelSettingsDto, DayOfWeek } from "services/backend/nswagts";

type Props = {
  submitCallback: (metaData: ChannelSettingsDto) => Promise<void>;
  channelId?: number;
};

const defaultChannel: ChannelSettingsDto = {
  groupSize: 2,
  startsDay: DayOfWeek.Thursday,
  weekRepeat: 1,
  durationInDays: 1,
  individualMessage: false
};

const ChannelMessagesForm: FC<Props> = ({ submitCallback, channelId }) => {
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
            <FormLabel>Round Start Channel Message</FormLabel>
            <Textarea
              placeholder="Round Start Channel Message"
              value={localFormData.roundStartChannelMessage}
              onChange={event => updateLocalForm(String(event.target.value), "roundStartChannelMessage")}
              />
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Round Start Group Message</FormLabel>
            <Textarea
              placeholder="Round Start Group Message"
              value={localFormData.roundStartGroupMessage}
              onChange={event => updateLocalForm(String(event.target.value), "roundStartGroupMessage")}
              />
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Round Midway Message</FormLabel>
            <Textarea
              placeholder="Round Midway Message"
              value={localFormData.roundMidwayMessage}
              onChange={event => updateLocalForm(String(event.target.value), "roundMidwayMessage")}
              />
          </FormControl>
          <FormControl isRequired>
            <FormLabel>Round Finisher Message</FormLabel>
            <Textarea
              placeholder="Round Finisher Message"
              value={localFormData.roundFinisherMessage}
              onChange={event => updateLocalForm(String(event.target.value), "roundFinisherMessage")}
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
export default ChannelMessagesForm;

