import { NextApiHandler } from "next";

const handler: NextApiHandler = async (req, res) => {
  const { photoUrl } = req.query;
  const { authorization } = req.headers;

  const token = (authorization as string).split("Bearer ")[1];

  const response = await fetch(photoUrl as string, {
    headers: {
      Authorization: "Bearer " + token
    },
    credentials: "omit"
  });

  const blob = await response.blob();

  res.writeHead(200, {
    "Content-Type": blob.type,
    "Content-Length": blob.size,
    "Cache-Control": "public, max-age=604800, immutable"
  });

  (blob.stream() as any).pipe(res);
};

export default handler;
