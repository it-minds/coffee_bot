import { NextPage } from "next";
import { useEffect } from "react";

const IndexPage: NextPage = () => {
  useEffect(() => {
    // get the URL parameters which will include the auth token
    const params = window.location.search;
    if (window.opener) {
      window.opener.postMessage(params, window.origin);
      window.close();
    }
  });
  // some text to show the user
  return <p>Please wait...</p>;
};

export default IndexPage;
