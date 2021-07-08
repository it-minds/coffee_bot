import { NextApiHandler } from "next";

const handler: NextApiHandler = async (req, res) => {
  const { photoUrl, token } = req.query;

  console.log(photoUrl, token);

  const response = await fetch(photoUrl as string, {
    headers: {
      Authorization: "Bearer " + token
    },
    credentials: "omit"
  });

  const blob = await response.blob();

  res.writeHead(200, {
    "Content-Type": blob.type,
    "Content-Length": blob.size
  });

  (blob.stream() as any).pipe(res);
};

export default handler;
