export const errorToJSON = (error: Error): string => {
  const errorRecord: Record<string, unknown> = {};
  Object.getOwnPropertyNames(error).forEach(function (key: keyof Error) {
    errorRecord[key] = error[key];
  }, error);

  return JSON.stringify(errorRecord);
};

declare global {
  interface Error {
    toJson(): string;
  }
}

Error.prototype.toJson = function () {
  return errorToJSON(this);
};
