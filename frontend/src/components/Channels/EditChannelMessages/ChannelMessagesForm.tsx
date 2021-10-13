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
  const [requiredTags, setRequiredTags] = useState<string[][]>(null);
  const [predicates, setPredicates] = useState<{ [key: string]: string }>(null);

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

  useEffectAsync(async () => {
    const client = await genClient();
    const tagData = await client.getRequiredTags();
    setPredicates(tagData.tagToPredicate);
    setRequiredTags([
      tagData.startChannelMessageRequiredTags,
      tagData.startGroupMessageRequiredTags,
      tagData.midwayMessageRequiredTags,
      tagData.finisherMessageRequiredTags
    ]);
  }, []);

  const onSubmit = useCallback(
    async event => {
      event.preventDefault();
      setIsLoading(true);
      if (validateFields()) await submitCallback(localFormData);
      setIsLoading(false);
    },
    [localFormData, requiredTags, predicates]
  );
  const updateLocalForm = useCallback((value: unknown, key: keyof ChannelSettingsDto) => {
    setLocalFormData(form => {
      (form[key] as unknown) = value;
      return { ...form };
    });
  }, []);

  const [validFields, setValidFields] = useState([true, true, true, true]);
  const validateFields = useCallback(() => {
    const valid = [true, true, true, true];

    requiredTags[0].forEach(tag => {
      valid[0] &&= RegExp(predicates[tag]).test(localFormData["roundStartChannelMessage"]);
    });
    requiredTags[1].forEach(tag => {
      valid[1] &&= RegExp(predicates[tag]).test(localFormData["roundStartGroupMessage"]);
    });
    requiredTags[2].forEach(tag => {
      valid[2] &&= RegExp(predicates[tag]).test(localFormData["roundMidwayMessage"]);
    });
    requiredTags[3].forEach(tag => {
      valid[3] &&= RegExp(predicates[tag]).test(localFormData["roundFinisherMessage"]);
    });

    setValidFields(valid);
    return valid[0] && valid[1] && valid[2] && valid[3];
  }, [localFormData, predicates, requiredTags]);

  return (
    <Flex w="full" align="center" justifyContent="center">
      <Box width="md" opacity={isLoading ? 0.2 : 1}>
        <Text>
          This is for changing the messages the coffee-bot will send to this channel. <br />
          The following tags can be used as variables in the messages:
        </Text>
        <UnorderedList my={5}>
          {predicates != null &&
            Object.keys(predicates).map(key => {
              return <ListItem key={key}>{key}</ListItem>;
            })}
        </UnorderedList>
        The following two only work in the &quot;Midway Message&quot;
        <UnorderedList>
          <ListItem>
            &#123;&#123; YesButton &#125;&#125;: Specifies where in the message the yes button will
            be
          </ListItem>
          <ListItem>
            &#123;&#123; NoButton &#125;&#125;: Specifies where in the message the no button will be
          </ListItem>
        </UnorderedList>
        <Text>In order to ping a group use &quot;&#60;!group to ping&#62;&quot;</Text>
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
              This message must contain the following tags:{" "}
              {requiredTags && requiredTags[0].join(", ")}
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
              This message must contain the following tags:{" "}
              {requiredTags && requiredTags[1].join(", ")}
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
              This message must contain the following tags:{" "}
              {requiredTags && requiredTags[2].join(", ")}
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
            <FormErrorMessage>
              This message must contain the following tags:{" "}
              {requiredTags && requiredTags[3].join(", ")}
            </FormErrorMessage>
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
