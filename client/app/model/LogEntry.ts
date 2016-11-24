import { LogType } from './LogType';

export class LogEntry {
  id: number;
  logType: LogType;
  textValue?: string;
  numberValue: number;
}