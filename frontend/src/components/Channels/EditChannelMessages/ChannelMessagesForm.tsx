import {
  Box,
  Button,
  Flex,
  FormControl,
  FormErrorMessage,
  FormLabel,
  ListItem,
  Text,
  Textarea,
  UnorderedList
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

  const onSubmit = useCallback(async event => {
    event.preventDefault();
    setIsLoading(true);
    if (validateFields()) await submitCallback(localFormData);
    setIsLoading(false);
  }, []);
  const updateLocalForm = useCallback((value: unknown, key: keyof ChannelSettingsDto) => {
    setLocalFormData(form => {
      (form[key] as unknown) = value;
      return { ...form };
    });
  }, []);

  const [validFields, setValidFields] = useState([true, true, true, true]);
  const validateFields = useCallback(() => {
    const valid = [true, true, true, true];

    valid[0] &&= /{{\s*[rR]ound[sS]tart[tT]ime\s*}}/g.test(
      localFormData["roundStartChannelMessage"]
    );
    valid[0] &&= /{{\s*[rR]ound[eE]nd[tT]ime\s*}}/g.test(localFormData["roundStartChannelMessage"]);
    valid[0] &&= /{{\s*[gG]roups\s*}}/g.test(localFormData["roundStartChannelMessage"]);

    valid[1] &&= /{{\s*[rR]ound[sS]tart[tT]ime\s*}}/.test(localFormData["roundStartGroupMessage"]);
    valid[1] &&= /{{\s*[rR]ound[eE]nd[tT]ime\s*}}/.test(localFormData["roundStartGroupMessage"]);

    valid[2] &&= /{{\s*[yY]es[bB]utton\s*}}/.test(localFormData["roundMidwayMessage"]);
    valid[2] &&= /{{\s*[nN]o[bB]utton\s*}}/.test(localFormData["roundMidwayMessage"]);

    setValidFields(valid);
    console.log(valid);
    return valid[0] && valid[1] && valid[2] && valid[3];
  }, [localFormData]);

  return (
    <Flex w="full" align="center" justifyContent="center">
      <Box width="md" opacity={isLoading ? 0.2 : 1}>
        <Text>
          This is for changing the messages the coffee-bot will send to this channel. <br />
          The following tags can be used as variables in the messages:
          <UnorderedList my={5}>
            <ListItem>
              &#123;&#123; MeetupPercentage &#125;&#125;: Will print the number representing the
              percent of people who have met up for coffee so far this round
            </ListItem>
            <ListItem>
              &#123;&#123; MeetupCondition &#125;&#125;: Will print the line &quot;Next time,
              let&apos;s try for 100% shall we?&quot; if MeetupPercentage is not 100%
            </ListItem>
            <ListItem>
              &#123;&#123; RoundStartTime &#125;&#125;: Will print the start date for the round
            </ListItem>
            <ListItem>
              &#123;&#123; RoundEndTime &#125;&#125;: Will print the end date for the round
            </ListItem>
            <ListItem>&#123;&#123; Groups &#125;&#125;: Wil print the list of all groups</ListItem>
          </UnorderedList>
          And the following two only work in the &quot;Midway Message&quot;
          <UnorderedList>
            <ListItem>
              &#123;&#123; YesButton &#125;&#125;: Specifies where in the message the yes button
              will be
            </ListItem>
            <ListItem>
              &#123;&#123; NoButton &#125;&#125;: Specifies where in the message the no button will
              be
            </ListItem>
          </UnorderedList>
          In order to ping a group use &quot;&#60;!group to ping&#62;&quot;
        </Text>
        <form onSubmit={onSubmit}>
          <FormControl mt={5} isRequired isInvalid={!validFields[0]}>
            <FormLabel>Round Start Channel Message</FormLabel>
            <Textarea
              placeholder="Round Start Channel Message"
              value={localFormData.roundStartChannelMessage}
              onChange={event =>
                updateLocalForm(String(event.target.value), "roundStartChannelMessage")
              }
            />
            <FormErrorMessage>
              This message must contain the following tags: &#123;&#123; RoundStartTime
              &#125;&#125;, &#123;&#123; RoundEndTime &#125;&#125; and &#123;&#123; Groups
              &#125;&#125;
            </FormErrorMessage>
          </FormControl>
          <FormControl isRequired isInvalid={!validFields[1]}>
            <FormLabel>Round Start Group Message</FormLabel>
            <Textarea
              placeholder="Round Start Group Message"
              value={localFormData.roundStartGroupMessage}
              onChange={event =>
                updateLocalForm(String(event.target.value), "roundStartGroupMessage")
              }
            />
            <FormErrorMessage>
              This message must contain the following tags: &#123;&#123; RoundStartTime &#125;&#125;
              and &#123;&#123; RoundEndTime &#125;&#125;
            </FormErrorMessage>
          </FormControl>
          <FormControl isRequired isInvalid={!validFields[2]}>
            <FormLabel>Round Midway Message</FormLabel>
            <Textarea
              placeholder="Round Midway Message"
              value={localFormData.roundMidwayMessage}
              onChange={event => updateLocalForm(String(event.target.value), "roundMidwayMessage")}
            />
            <FormErrorMessage>
              This message must contain the following tags: &#123;&#123; YesButton &#125;&#125; and
              &#123;&#123; NoButton &#125;&#125;
            </FormErrorMessage>
          </FormControl>
          <FormControl isRequired isInvalid={!validFields[3]}>
            <FormLabel>Round Finisher Message</FormLabel>
            <Textarea
              placeholder="Round Finisher Message"
              value={localFormData.roundFinisherMessage}
              onChange={event =>
                updateLocalForm(String(event.target.value), "roundFinisherMessage")
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
export default ChannelMessagesForm;
