import {
  Button,
  FormControl,
  FormErrorMessage,
  FormHelperText,
  FormLabel,
  Input,
  Switch
} from "@chakra-ui/react";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { FC, useCallback, useState } from "react";
import { useForm } from "react-hook-form";
import { ChannelClient, UpdateChannelPauseInput } from "services/backend/nswagts";

type Props = {
  channelId: number;
  onSuccess?: () => void;
};

const AdjustChannelPauseForm: FC<Props> = ({ channelId, onSuccess = () => null }) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
    watch,
    setValue
  } = useForm<UpdateChannelPauseInput>();

  const [loading, setLoading] = useState(false);

  const { genClient } = useNSwagClient(ChannelClient);

  useEffectAsync(async () => {
    setLoading(true);
    const client = await genClient();
    const settings = await client.getMyChannelMembership(channelId);

    reset({
      paused: settings.onPause,
      unPauseDate: new Date(settings.returnFromPauseDate?.toISOString().substring(0, 10))
    });
    setLoading(false);
  }, []);

  const onSubmit = useCallback(
    async (data: UpdateChannelPauseInput) => {
      setLoading(true);
      data.channelId = channelId;
      data.unPauseDate && (data.unPauseDate = new Date(data.unPauseDate));

      const client = await genClient();
      await client.updateChannelState({
        input: data
      });

      onSuccess();
      setLoading(false);
    },
    [channelId]
  );
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <FormControl id="paused" isInvalid={!!errors.paused}>
        <FormLabel>Participating</FormLabel>
        <FormHelperText>
          Choosing to not participate does not remove you from the current round only future.
        </FormHelperText>
        <Switch
          isReadOnly={loading}
          isChecked={!watch("paused")}
          onChange={x => setValue("paused", !x.target.checked)}
        />
        <FormErrorMessage>Titles length must be between 3 and 30 characters.</FormErrorMessage>
      </FormControl>

      <FormControl id="unPauseDate" isInvalid={!!errors.unPauseDate} mt={2}>
        <FormLabel>Re-Participating Date</FormLabel>
        <FormHelperText>
          You can choose to have your participation automatically re-enabled by selecting the date
          to re-participate.
        </FormHelperText>
        <Input
          isReadOnly={loading}
          type="date"
          {...register("unPauseDate", { required: false })}
          min={new Date().toISOString().substring(0, 10)}
        />
        <FormErrorMessage>Titles length must be less than 3000 characters.</FormErrorMessage>
      </FormControl>

      <Button type="submit" colorScheme="green" isLoading={loading} mt={[4, 6]}>
        Update Participation Status
      </Button>
    </form>
  );
};

export default AdjustChannelPauseForm;
