import Demo from "components/Demo/Demo";
import { AuthContext } from "contexts/AuthContext";
import { Locale } from "i18n/Locale";
import { GetStaticProps, NextPage } from "next";
import { I18nProps } from "next-rosetta";
import { useContext } from "react";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);

  // return <Demo />;
  return (
    <div>
      <h1>Hi you should be</h1>
      <pre>{JSON.stringify(activeUser, null, 2)}</pre>
    </div>
  );
};

export const getStaticProps: GetStaticProps<I18nProps<Locale>> = async context => {
  const locale = context.locale || context.defaultLocale;
  const { table = {} } = await import(`../i18n/${locale}`);

  return {
    props: { table }
  };
};

export default IndexPage;
