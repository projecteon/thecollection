export enum UserFeedbackType {
  Error,
  Information,
  Warning,
}

export interface IUserFeedback {
  id: number;
  type: UserFeedbackType;
  message: string;
}
