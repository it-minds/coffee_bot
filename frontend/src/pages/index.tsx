import { AuthContext } from "contexts/AuthContext";
import { Locale } from "i18n/Locale";
import { GetStaticProps, NextPage } from "next";
import { I18nProps } from "next-rosetta";
import { useCallback, useReducer } from "react";
import { useEffect } from "react";
import { useContext } from "react";
import ListReducer, { ListReducerActionType } from "react-list-reducer";
import { genChannelClient } from "services/backend/apiClients";
import { IChannelSettingsIdDto } from "services/backend/nswagts";
import { logger } from "utils/logger";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const [channels, dispatchChannels] = useReducer(ListReducer<IChannelSettingsIdDto>("id"), []);

  const fetchChannels = useCallback(async () => {
    try {
      const client = await genChannelClient();
      const data = await client.getMyChannels();
      if (data && data.length >= 0) {
        dispatchChannels({
          type: ListReducerActionType.Reset,
          data
        });
      } else logger.info("ApplicationClient.getAppTokensICanReview got no data");
    } catch (err) {
      logger.warn("ApplicationClient.getAppToken Error", err);
    }
  }, []);

  useEffect(() => {
    fetchChannels();
  }, []);

  // return <Demo />;
  return (
    <div>
      <h1>Hi you should be</h1>
      <pre>{JSON.stringify(activeUser, null, 2)}</pre>
    </div>
  );
};

export default IndexPage;
